class Vip {
  final int vipID;
  final String vipType;
  final String vipStatus;
  final int vipCost;
  final int vipDiscount;
  final String vipImage;

  Vip({
    required this.vipID,
    required this.vipType,
    required this.vipStatus,
    required this.vipCost,
    required this.vipDiscount,
    required this.vipImage,
  });

  factory Vip.fromJson(Map<String, dynamic> json) {
    return Vip(
      vipID: json['vipID'] as int,
      vipType: json['vipType'] as String,
      vipStatus: json['vipStatus'] as String,
      vipCost: json['vipCost'] as int,
      vipDiscount: json['vipDiscount'] as int,
      vipImage: json['vipImage'] as String,
    );
  }

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