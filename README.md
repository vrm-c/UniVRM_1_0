# UniVRM に引っ越し

既存のUniVRMとコードを共有したいので、

`https://github.com/vrm-c/UniVRM`

に同居することにしました。

`https://github.com/vrm-c/UniVRM_1_0/commit/3138de1fd98550b9879bc96857e95d468e46f537` を移植しました。

* https://github.com/vrm-c/UniVRM/issues?q=is%3Aissue+is%3Aopen+label%3A1.0

UPMパッケージは変更になります。

* https://github.com/vrm-c/UniVRM/releases/tag/v0.64.0

# UniVRM-1.0 draft

VRM-1.0 draft の Unity 実装になります。

* `draft` 仕様が変更される可能性高いため、正式版が出るまではプロダクト等で使用しないでください

* SupportUnityVersion: `Unity2019.3`
* 推奨ColorSpaceSetting: `Linear`

[開発Wiki](https://github.com/vrm-c/UniVRM_1_0/wiki)

## UPM

* 20200902 パッケージ名変更( `com.vrmc.univrm` => `com.vrmc.univrm1` )

```json
"com.vrmc.vrmshaders": "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders#v0.56.0",
"com.vrmc.meshutility": "https://github.com/vrm-c/UniVRM.git?path=/Assets/MeshUtility#v0.59.0",
"com.vrmc.univrm": "https://github.com/vrm-c/UniVRM_1_0.git?path=/Assets/Vrm10#__TAGNAME__", // __TAGNAME__ を適宜置き換え
```

## 依存ライブラリ

VrmLibがバイト列操作で `System.Memory` や `System.Span` に依存(`.NETStandard-2.1`から標準ライブラリに入る)

* /Assets/dotnet.system.memory/Runtime
    * System.Memory.dll
    * System.Buffers.dll
    * System.Runtime.CompilerServices.Unsafe.dll
* https://www.nuget.org/packages/System.Memory/
* https://github.com/dotnet/corefx/blob/master/LICENSE.TXT

ProtobufSerializerのJSONの読み書きが、 `Google.Protobuf` のJSONシリアライザーに依存

* /Assets/ProtobufSerializer/Google.Protobuf 
* https://github.com/protocolbuffers/protobuf/tree/master/csharp/src/Google.Protobuf 
* https://github.com/protocolbuffers/protobuf/blob/master/LICENSE

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
 ^                 |
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
