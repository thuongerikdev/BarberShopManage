import 'package:barbermanagemobile/domain/entities/vip.dart';

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
      vipID: json['vipID'] as int,
      vipType: json['vipType'] as String,
      vipStatus: json['vipStatus'] as String,
      vipCost: json['vipCost'] as int,
      vipDiscount: json['vipDiscount'] as int,
      vipImage: json['vipImage'] as String,
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
}