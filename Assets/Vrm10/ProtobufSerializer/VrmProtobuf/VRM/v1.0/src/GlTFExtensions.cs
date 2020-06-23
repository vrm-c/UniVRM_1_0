// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: glTF_extensions.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VrmProtobuf {

  /// <summary>Holder for reflection information generated from glTF_extensions.proto</summary>
  public static partial class GlTFExtensionsReflection {

    #region Descriptor
    /// <summary>File descriptor for glTF_extensions.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static GlTFExtensionsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVnbFRGX2V4dGVuc2lvbnMucHJvdG8SC1ZybVByb3RvYnVmGg5WUk1DX3Zy",
            "bS5wcm90bxoVVlJNQ19zcHJpbmdCb25lLnByb3RvGhVWUk1DX2NvbnN0cmFp",
            "bnQucHJvdG8ioAEKCkV4dGVuc2lvbnMSJgoIVlJNQ192cm0YASABKAsyFC5W",
            "cm1Qcm90b2J1Zi5WUk1DVnJtEjQKD1ZSTUNfc3ByaW5nQm9uZRgCIAEoCzIb",
            "LlZybVByb3RvYnVmLlZSTUNTcHJpbmdCb25lEjQKD1ZSTUNfY29uc3RyYWlu",
            "dBgDIAEoCzIbLlZybVByb3RvYnVmLlZSTUNDb25zdHJhaW50YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::VrmProtobuf.VRMCVrmReflection.Descriptor, global::VrmProtobuf.VRMCSpringBoneReflection.Descriptor, global::VrmProtobuf.VRMCConstraintReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VrmProtobuf.Extensions), global::VrmProtobuf.Extensions.Parser, new[]{ "VRMCVrm", "VRMCSpringBone", "VRMCConstraint" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Extensions : pb::IMessage<Extensions> {
    private static readonly pb::MessageParser<Extensions> _parser = new pb::MessageParser<Extensions>(() => new Extensions());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Extensions> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrmProtobuf.GlTFExtensionsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Extensions() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Extensions(Extensions other) : this() {
      vRMCVrm_ = other.vRMCVrm_ != null ? other.vRMCVrm_.Clone() : null;
      vRMCSpringBone_ = other.vRMCSpringBone_ != null ? other.vRMCSpringBone_.Clone() : null;
      vRMCConstraint_ = other.vRMCConstraint_ != null ? other.vRMCConstraint_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Extensions Clone() {
      return new Extensions(this);
    }

    /// <summary>Field number for the "VRMC_vrm" field.</summary>
    public const int VRMCVrmFieldNumber = 1;
    private global::VrmProtobuf.VRMCVrm vRMCVrm_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrmProtobuf.VRMCVrm VRMCVrm {
      get { return vRMCVrm_; }
      set {
        vRMCVrm_ = value;
      }
    }

    /// <summary>Field number for the "VRMC_springBone" field.</summary>
    public const int VRMCSpringBoneFieldNumber = 2;
    private global::VrmProtobuf.VRMCSpringBone vRMCSpringBone_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrmProtobuf.VRMCSpringBone VRMCSpringBone {
      get { return vRMCSpringBone_; }
      set {
        vRMCSpringBone_ = value;
      }
    }

    /// <summary>Field number for the "VRMC_constraint" field.</summary>
    public const int VRMCConstraintFieldNumber = 3;
    private global::VrmProtobuf.VRMCConstraint vRMCConstraint_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrmProtobuf.VRMCConstraint VRMCConstraint {
      get { return vRMCConstraint_; }
      set {
        vRMCConstraint_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Extensions);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Extensions other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(VRMCVrm, other.VRMCVrm)) return false;
      if (!object.Equals(VRMCSpringBone, other.VRMCSpringBone)) return false;
      if (!object.Equals(VRMCConstraint, other.VRMCConstraint)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (vRMCVrm_ != null) hash ^= VRMCVrm.GetHashCode();
      if (vRMCSpringBone_ != null) hash ^= VRMCSpringBone.GetHashCode();
      if (vRMCConstraint_ != null) hash ^= VRMCConstraint.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (vRMCVrm_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(VRMCVrm);
      }
      if (vRMCSpringBone_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(VRMCSpringBone);
      }
      if (vRMCConstraint_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(VRMCConstraint);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (vRMCVrm_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(VRMCVrm);
      }
      if (vRMCSpringBone_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(VRMCSpringBone);
      }
      if (vRMCConstraint_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(VRMCConstraint);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Extensions other) {
      if (other == null) {
        return;
      }
      if (other.vRMCVrm_ != null) {
        if (vRMCVrm_ == null) {
          VRMCVrm = new global::VrmProtobuf.VRMCVrm();
        }
        VRMCVrm.MergeFrom(other.VRMCVrm);
      }
      if (other.vRMCSpringBone_ != null) {
        if (vRMCSpringBone_ == null) {
          VRMCSpringBone = new global::VrmProtobuf.VRMCSpringBone();
        }
        VRMCSpringBone.MergeFrom(other.VRMCSpringBone);
      }
      if (other.vRMCConstraint_ != null) {
        if (vRMCConstraint_ == null) {
          VRMCConstraint = new global::VrmProtobuf.VRMCConstraint();
        }
        VRMCConstraint.MergeFrom(other.VRMCConstraint);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (vRMCVrm_ == null) {
              VRMCVrm = new global::VrmProtobuf.VRMCVrm();
            }
            input.ReadMessage(VRMCVrm);
            break;
          }
          case 18: {
            if (vRMCSpringBone_ == null) {
              VRMCSpringBone = new global::VrmProtobuf.VRMCSpringBone();
            }
            input.ReadMessage(VRMCSpringBone);
            break;
          }
          case 26: {
            if (vRMCConstraint_ == null) {
              VRMCConstraint = new global::VrmProtobuf.VRMCConstraint();
            }
            input.ReadMessage(VRMCConstraint);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code