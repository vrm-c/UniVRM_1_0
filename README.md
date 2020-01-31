# UniVRM-1.0 draft

VRM-1.0 draft の Unity 実装になります。
※draftは仕様が変更される可能性高いため、正式版が出るまではプロダクト等で使用しないでください

※大部分が新実装に置き換わっているため、0.X系であった機能が無くなっている場合はissueで報告して頂けると助かります

※このリポジトリではVRM1.0のEditor&Runtime用のCoreライブラリだけが入る予定です、その他のUtilityに関しては別のリポジトリで管理するようにします

SupportUnityVersion: Unity2019.3
ColorSpaceSetting: Linear

## Implementation
```
+-------------------+
| GameObject        |
| Assets            |左手系Y-UP
+-------------------+
 ^                 |
 |[UnityBuilder]   |[VrmConverter]
 |                 |
 Unity-2019.3      |
===============================================
.NET Standard-2.0 + System.Memory
 |                 |
 |                 |
 | 右手系Y-UP      v  
+-------------------+
|VrmLib.Model       |
+-------------------+
 ^                 |  [import]
 | VrmLib          |
 |[import]         v[export]
+-------------------+
|VrmProtobuf        |
+-------------------+
 ^                 |
 | Protobuf        v
+-------------------+
|GLB(GLTF+BIN)      |VRM-1.0
+-------------------+
```

## 構成

### VrmLib(namespace VrmLib)

GLTFからバイト列を切り出して、index 参照を実体化して作業しやすくした中間の入れ物 `VrmLib.Model`。

### ProtobufSerializer

VRM-1.0 の読み書き。
ProtocolBufferで定義して、Jsonのシリアライザを生成したもの。

#### Google.Protobuf

Google.Protobuf

#### VrmProtobuf(namespace VrmProtobuf)

GLTFの Protobuf 定義から出力した C# コンテナとJSONシリアライザー。
`JSON(VRM-1.0) => C#(VrmProtobuf)` と `C#(VrmProtobuf) => JSON(VRM-1.0)` を実装する。

#### ProtobufSerializer(namespace VrmProtobuf)

C#にシリアライズされた入れ物から、中間形式の `VrmLib.Model` に移し替える。
`VrmProtobuf => VrmLib.Model` と `VrmLib.Model => VrmProtobuf` を実装する。

### Builder(namespace UniVRM10)
UnityBuilder               VRMBuilder
`VrmLib.Model => Unity` と `Unity => VrmLib.Model` を実装する。

* 右手系・左手系の変換
* スクリプトスレッドで実行する必要あり
* GLTF部分(Mesh, Texture, Material, GameObject, SkinnedMeshRendererなど)
* VRM部分(HumanoidAvatar, BlendShapeProxy, LookAt, FirstPersonなど)

### 実装状況
#### File IO
- [x] spec1.0形式のRuntime&Editorエクスポート
- [x] spec1.0形式のRuntime&Editorインポート
- [ ] spec0.XのImport
- [x] Editorインポート時はScriptedImporterを使用する
- [X] Export時に強制で正規化をかける
- [ ] TPoseになっているかチェックする機能 
#### Material
- [X] MToonMaterialのインポート、エクスポート
- [X] UnlitMaterialのインポート、エクスポート
- [ ] PbrMaterialのインポート、エクスポート
- [X] ColorPropertyをLinearに統一
- [X] MapTextureをLinearて取り扱うように修正
- [ ] jpegテクスチャ対応
- [ ] 入出力テストの追加
### BlendShape
- [X] Blink, LookAt, Mouthの排他設定を追加
- [X] LookAtの対象はHead決め打ちに変更
- [X] LookAtの座標系を右手系で出力するように修正
- [ ] MaterialBindの仕様を整理する
- [ ] FirstParsonのMesh指定をNodeIndexに変更する
- [ ] BlendShapeBindの指定をNodeIndexに変更する
### SpringBone
- [X] 座標系を右手系で出力するように修正
- [ ] SpringBoneにカプセルコライダを追加する
- [ ] 単独の拡張に移動させる(VRMC_springBone)
### Constraint
- [ ] Constraintの追加
### Schema 
- [ ] metaの内容変更に対応する
### Gltf拡張対応
- [X] KHR_materials_unlit
- [ ] KHR_texture_transform
- [ ] KHR_materials_pbrSpecularGlossiness
- [ ] MSFT_lod
- [ ] KHR_compressed_texture_transmission?
### Other
- [ ] IL2CPPのサポート、AOTの動作確認
