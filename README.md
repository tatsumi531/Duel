# Duel

1対1の対戦アクションゲーム。ポートフォリオ用途および敵AI実装の学習用に個人開発中。
UnityRoomへの投稿を予定。

## 概要

- ジャンル: 1v1対戦アクション（2D）
- エンジン: Unity 6000.5.5f1
- 開発形態: 個人開発
- 目的: 転職活動用ポートフォリオ／敵AI実装の学習

## 技術スタック

- Unity 2D（Sprite / SpriteRenderer ベース）
- DOTween（演出のトゥイーン処理）
- Unity Input System

## アーキテクチャ

### ゲーム進行（Stateパターン）

[GameStateMachine.cs](Assets/Duel/Scripts/GameStateMachine.cs) が `IGameState` を実装した各ステートを保持し、`Update()` で現在のステートに処理を委譲するだけの薄いドライバになっている。

[GameState.cs](Assets/Duel/Scripts/GameState.cs) の `GameStateBase` が共通処理を担う:
- ステート切り替え時に `CancellationTokenSource` を発行・破棄し、非同期処理の後始末を自動化
- `RunFlow(IEnumerator)` でコルーチンを起動し、フェードやラウンド初期化など時間軸を伴う演出を「Flow」として記述できるようにしている

ステート遷移:

```
Boot → TitleScreen → InitializeRound → RoundIntroduction → Round → Win/Lose → InitializeRound（ループ）
```

### キャラクター

- [Character.cs](Assets/Duel/Scripts/Character/Character.cs): 移動・攻撃エフェクト生成・被弾判定（`OnTriggerEnter2D` で `Slash` との衝突を検知）などMonoBehaviourとしての実体を担当
- [CharacterStateMachine.cs](Assets/Duel/Scripts/Character/CharacterStateMachine.cs) / [CharacterState.cs](Assets/Duel/Scripts/Character/CharacterState.cs): Idle / Forward / Back / Attack / Hit / Dead の状態遷移を管理
- [CharacterData.cs](Assets/Duel/Scripts/Character/CharacterData.cs): 移動速度・攻撃エフェクトのオフセットなどをScriptableObjectで外出し
- プレイヤー（[Player.cs](Assets/Duel/Scripts/Character/Player.cs) / [PlayerStateMachine.cs](Assets/Duel/Scripts/Character/PlayerStateMachine.cs)）と敵（[Enemy.cs](Assets/Duel/Scripts/Character/Enemy.cs) / [EnemyStateMachine_Reactive.cs](Assets/Duel/Scripts/Character/EnemyStateMachine_Reactive.cs)）で共通の状態機械基盤を利用

現状は体力（HP）の概念がなく、被弾1回でラウンド決着となる仕様。

### 敵AI

[EnemyAIProfile.cs](Assets/Duel/Scripts/Character/EnemyAIProfile.cs) はScriptableObjectとして敵AIの行動パラメータを外出しした設計。

- 攻撃間合い（`attackRange`）と維持したい距離（`preferredDistance` / `distanceTolerance`）で距離帯を判定
- 一定間隔（`reactionTimeMin` 〜 `reactionTimeMax` のランダム値）ごとに行動を再抽選する方式で反応速度にブレを持たせている
- 距離帯ごとに攻撃・後退・接近の確率（`attackChance` / `retreatChance` / `approachChance`）を設定でき、プロファイルを複数用意することでAIの「性格」を差し替え可能

有限ステートマシンやビヘイビアツリーではなく、距離帯判定＋確率抽選によるシンプルな意思決定モデル。パラメータ調整・プロファイル差し替えのしやすさを優先した設計。

## ディレクトリ構成

```
Assets/Duel/
├── Data/                 # ScriptableObjectのアセットデータ
├── Scenes/               # Main.unity
├── Scripts/
│   ├── Character/        # プレイヤー・敵・共通の状態機械、AIプロファイル
│   ├── UI/               # タイトル・ラウンド導入・結果画面
│   └── (root)/           # GameState、ラウンド進行、フェード等の全体制御
```

## 既知の課題 / 今後の対応予定

- 体力（HP）システムが無く一撃即決着。将来的にダメージ量・複数回被弾の仕組みを検討
- 自動テストが未整備

## 開発ログ

<!-- 開発が進むごとにここへ追記していく -->
