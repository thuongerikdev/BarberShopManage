import 'package:barbermanagemobile/domain/entities/vip/vip.dart';

class VipModel extends Vip {
  VipModel({
    required int vipID,
    required String vipType,
    required String vipStatus,
    required int vipCost,
    required int vipDiscount,
    required String vipImage,
  }) : super(
          vipID: vipID,
          vipType: vipType,
          vipStatus: vipStatus,
          vipCost: vipCost,
          vipDiscount: vipDiscount,
          vipImage: vipImage,
        );

  factory VipModel.fromJson(Map<String, dynamic> json) {
    return VipModel(
      vipID: json['vipID'] as int? ?? 0,
      vipType: json['vipType'] as String? ?? '',
      vipStatus: json['vipStatus'] as String? ?? '',
      vipCost: json['vipCost'] as int? ?? 0,
      vipDiscount: json['vipDiscount'] as int? ?? 0,
      vipImage: json['vipImage'] as String? ?? '',
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'vipID': vipID,
      'vipType': vipType,
      'vipStatus': vipStatus,
      'vipCost': vipCost,
      'vipDiscount': vipDiscount,
      'vipImage': vipImage,
    };
  }

  Vip toEntity() {
    return Vip(
      vipID: vipID,
      vipType: vipType,
      vipStatus: vipStatus,
      vipCost: vipCost,
      vipDiscount: vipDiscount,
      vipImage: vipImage,
    );
  }
}