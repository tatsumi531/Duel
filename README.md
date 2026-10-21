# Duel

1対1の対戦アクションゲーム。ポートフォリオ用途および敵AI実装の学習用に個人開発中。
UnityRoomへの投稿を予定。

## 概要

- ジャンル: 1v1対戦アクション（2D）
- エンジン: Unity 6000.5.5f1
- 開発形態: 個人開発
- 目的: 転職活動用ポートフォリオ／敵AI実装の学習

## 技術スタック

- Unity 2D（Sprite / SpriteRenderer ベース、URP）
- Unity Input System（キーボード / マウス入力）
- DOTween（ノックバックやUI演出のトゥイーン処理）
- Unity 6の `Awaitable` + `CancellationToken`（ゲーム進行フロー全体の非同期制御）
- カスタムシェーダー（[SpriteSlice.shader](Assets/Duel/Shaders)）によるヒット時のスプライト切断エフェクト
- Unity Test Framework（純粋ロジックのEditModeテスト）

## アーキテクチャ

### ゲーム進行（Stateパターン）

[GameStateMachine.cs](Assets/Duel/Scripts/GameStateMachine.cs) が `IGameState` を実装した各ステートを保持し、`Update()` で現在のステートに処理を委譲するだけの薄いドライバになっている。

[GameState.cs](Assets/Duel/Scripts/GameState.cs) の `GameStateBase` が共通処理を担う:
- ステート切り替え時に `CancellationTokenSource` を発行・破棄し、非同期処理の後始末を自動化
- `RunFlow(Func<CancellationToken, Awaitable>)` で時間軸を伴う処理を「Flow」として起動し、フェード・待機・UI演出を `await` で直列に記述できるようにしている
- 非同期の境界となる `async void` を `RunFlow` の1箇所に閉じ込め、キャンセル例外のみそこで処理する。それ以外の例外は握り潰さずConsoleへ出す方針
- DOTweenのトゥイーンは[TweenAwaitableExtensions.cs](Assets/Duel/Scripts/TweenAwaitableExtensions.cs)で `Awaitable` に変換し、完了待ちも `await` に統一している

時間軸を伴う処理はコルーチンを使わず `Awaitable` に統一している。一方、ノックバックのように単発で完結するキャラクター演出はDOTweenのコールバックで処理しており、キャラクター側のステートマシンまで作り替える影響範囲と見合わないため、統一の対象外としている。

ステート遷移:

```
Boot → TitleScreen → InitializeRound → RoundIntroduction → Round → Win → InitializeRound（プレイヤー勝利時、次の敵とのラウンドへループ）
                                                                  → Lose → TotalResult → TitleScreen（プレイヤー敗北時）
```

`Boot` でプレイヤーと敵のステートマシンに互いの参照を渡し、`InitializeRound` で[EnemyRoster.cs](Assets/Duel/Scripts/Character/EnemyRoster.cs)からラウンド数に応じた敵データを取得して初期化する。キャラクターの初期位置と移動可能範囲は[BattleField.cs](Assets/Duel/Scripts/BattleField.cs)が保持する値を基準にしている。

### キャラクター

- [Character.cs](Assets/Duel/Scripts/Character/Character.cs): 移動・攻撃エフェクト（[Slash.cs](Assets/Duel/Scripts/Slash.cs)）の生成・攻撃レンジ計算などMonoBehaviourとしての実体を担当
- [CharacterStateMachine.cs](Assets/Duel/Scripts/Character/CharacterStateMachine.cs) / [CharacterState.cs](Assets/Duel/Scripts/Character/CharacterState.cs): Neutral / Idle / Forward / Back / Attack / KnockBack / Hit / NoneActive の状態遷移を管理
- [CharacterController.cs](Assets/Duel/Scripts/Character/CharacterController.cs): `ICharacterController` を実装した `PlayerController`（キーボード/マウス入力）と `AIController`（AIプロファイルに基づく意思決定）を切り替え可能にし、プレイヤーと敵でステートマシン本体を共用
- [CharacterAnimatorController.cs](Assets/Duel/Scripts/Character/CharacterAnimatorController.cs): Animatorパラメータの設定をラップし、ステート側からAnimatorの詳細を隠蔽
- [CharacterData.cs](Assets/Duel/Scripts/Character/CharacterData.cs): 移動速度・攻撃エフェクトのオフセット・アニメーターのOverrideControllerなどをScriptableObjectで外出し
- [Slash.cs](Assets/Duel/Scripts/Slash.cs): 攻撃同士が重なった場合は相殺（KnockBack）、キャラクターに当たった場合は被弾（Hit）を判定し、[SpriteSliceEffect.cs](Assets/Duel/Scripts/Character/SpriteSliceEffect.cs)でスプライトを2分割して吹っ飛ばす演出を再生

現状は体力（HP）の概念がなく、被弾1回でラウンド決着となる仕様。

### 敵AI

[CharacterAIProfile.cs](Assets/Duel/Scripts/Character/CharacterAIProfile.cs) はScriptableObjectとして敵AIの行動パラメータを外出しした設計。[AIController](Assets/Duel/Scripts/Character/CharacterController.cs)がこのプロファイルを読み、`Character.GetAttackRange()`（攻撃エフェクトの当たり判定サイズ＋相手の当たり判定幅から動的に算出）を基準に距離帯を判定する。

- 一定間隔（`reactionTimeMin` 〜 `reactionTimeMax` のランダム値）ごとに行動を再抽選する方式で反応速度にブレを持たせている
- 相手の攻撃範囲＋`preferredDistanceMargin`を「維持したい間合い」とし、そこから`distanceTolerance`を超えて外れると近づく／離れるを選択
- 間合いや攻撃範囲の内外に応じて攻撃・後退・接近の確率（`attackChance` / `retreatChance` / `approachChance`）を抽選し、プロファイルを複数用意することでAIの「性格」を差し替え可能（現在8種の敵キャラ分を用意）
- 「詰める」と一度判断したら攻撃範囲に入るまで前進をやめない`rushing`フラグを持ち、安全な間合いと危険な間合いの間で判断がぶれて足踏みし続ける問題を回避

有限ステートマシンやビヘイビアツリーではなく、距離帯判定＋確率抽選によるシンプルな意思決定モデル。パラメータ調整・プロファイル差し替えのしやすさを優先した設計。

行動選択そのものは[AIDecision.cs](Assets/Duel/Scripts/Core/AIDecision.cs)に純粋関数として分離している。距離・攻撃範囲・抽選値・`rushing`フラグを引数で受け取って結果を返す形にしたため、`AIController`は「状況を集めて渡し、結果をステートマシンへ反映する」だけの薄い層になり、乱数を含む判断をテストで決定論的に検証できる。

### テスト

ロジックのうちMonoBehaviourに依存しない部分を `Duel.Core` アセンブリ（[Assets/Duel/Scripts/Core](Assets/Duel/Scripts/Core)）に分離し、EditModeテスト（[Assets/Duel/Tests](Assets/Duel/Tests)）で検証している。

- [AIDecision.cs](Assets/Duel/Scripts/Core/AIDecision.cs): 距離帯・抽選値・`rushing`フラグの組み合わせごとの行動選択
- [RoundCounter.cs](Assets/Duel/Scripts/Core/RoundCounter.cs) / [RoundTimer.cs](Assets/Duel/Scripts/Core/RoundTimer.cs): ラウンド進行と経過時間の管理
- [ElapsedTimeFormatter.cs](Assets/Duel/Scripts/Core/ElapsedTimeFormatter.cs): 経過時間の表示整形（分と秒の境界の丸め）

テストを書けるようにするため、乱数や `Time.deltaTime` のように毎回値が変わるものは関数の内部で取得せず、引数で受け取る形に変更している。乱数を振るのは `AIController` 側、判断するのは `AIDecision` 側、という分担にすることで、AIの意思決定を固定値で検証できるようにした。

実行方法: Unityエディタの Window > General > Test Runner から EditMode タブを選択して Run All。

演出・入力・衝突判定などMonoBehaviourに依存する部分はテスト対象外とし、実機確認で担保している。

## ディレクトリ構成

```
Assets/Duel/
├── Animations/            # 各キャラクターのAnimator / AnimatorOverrideController
├── Data/                  # ScriptableObjectのアセットデータ（CharacterData, CharacterAIProfile, EnemyRoster, GameData）
├── Fonts/                 # 日本語表示用のサブセットフォント
├── Prefabs/               # キャラクター・エフェクト等のプレハブ
├── Resources/             # スプライト切断エフェクト用マテリアル
├── Scenes/                # Main.unity（本編）, AnimationDemo.unity（アニメーション確認用）
├── Shaders/               # スプライト切断エフェクト用シェーダー
├── Sounds/                # 効果音
├── Scripts/
│   ├── Core/              # MonoBehaviourに依存しない純粋ロジック（Duel.Coreアセンブリ）
│   ├── Character/         # キャラクター本体、ステートマシン、コントローラー、AIプロファイル
│   ├── UI/                # タイトル・ラウンド導入・結果画面
│   └── (root)/            # GameState、フェード、斬撃判定等の全体制御
└── Tests/                 # EditModeテスト（Duel.Tests.EditModeアセンブリ）
```

## 使用アセットについて

キャラクター・背景スプライトおよびエフェクトの一部は、Unity Asset Storeで購入した以下のアセットを使用しています（[Assets/Store](Assets/Store)配下）。ゲームロジック・ステートマシン・AI・UI制御などのスクリプトは全て自作です。

- Fantasy Martial Characters 3
- Hero_Knight_2
- Martial Hero
- FREE Parallax Forest Background HQ
- JMO Assets / Cartoon FX Remaster

## 既知の課題 / 今後の対応予定

- 体力（HP）システムが無く一撃即決着。将来的にダメージ量・複数回被弾の仕組みを検討
- 自動テストの対象は `Duel.Core` の純粋ロジックに限定。MonoBehaviour側は実機確認で担保している
- プレイ可能なWebGLビルドをUnityRoomへ未投稿（予定）

## 開発ログ

<!-- 開発が進むごとにここへ追記していく -->
