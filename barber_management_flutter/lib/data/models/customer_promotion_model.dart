import 'package:barbermanagemobile/domain/entities/customer_promotion.dart';

class CustomerPromotionModel extends CustomerPromotion {
  CustomerPromotionModel({
    required int promoID,
    required String promoName,
    required String promoDescription,
    required double promoDiscount,
    required DateTime promoStart,
    required DateTime promoEnd,
    required String promoStatus,
    required String promoType,
    required String promoImage,
  }) : super(
          promoID: promoID,
          promoName: promoName,
          promoDescription: promoDescription,
          promoDiscount: promoDiscount,
          promoStart: promoStart,
          promoEnd: promoEnd,
          promoStatus: promoStatus,
          promoType: promoType,
          promoImage: promoImage,
        );

  factory CustomerPromotionModel.fromJson(Map<String, dynamic> json) {
    return CustomerPromotionModel(
      promoID: json['promoID'] as int,
      promoName: json['promoName'] as String,
      promoDescription: json['promoDescription'] as String,
      promoDiscount: (json['promoDiscount'] as num).toDouble(),
      promoStart: DateTime.parse(json['promoStart'] as String),
      promoEnd: DateTime.parse(json['promoEnd'] as String),
      promoStatus: json['promoStatus'] as String,
      promoType: json['promoType'] as String,
      promoImage: json['promoImage'] as String,
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'promoID': promoID,
      'promoName': promoName,
      'promoDescription': promoDescription,
      'promoDiscount': promoDiscount,
      'promoStart': promoStart.toIso8601String(),
      'promoEnd': promoEnd.toIso8601String(),
      'promoStatus': promoStatus,
      'promoType': promoType,
      'promoImage': promoImage,
    };
  }
}