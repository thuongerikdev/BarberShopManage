import 'package:barbermanagemobile/domain/entities/promotion.dart';

class PromotionModel extends Promotion {
  PromotionModel({
    required int promoID,
    required String promoName,
    required String promoDescription,
    required double promoDiscount,
    required int pointToGet,
    required DateTime promoStart,
    required DateTime promoEnd,
    required String promoStatus,
    required String promoType,
    required String promoImage,
    List<dynamic>? authCusPromos,
  }) : super(
          promoID: promoID,
          promoName: promoName,
          promoDescription: promoDescription,
          promoDiscount: promoDiscount,
          pointToGet: pointToGet,
          promoStart: promoStart,
          promoEnd: promoEnd,
          promoStatus: promoStatus,
          promoType: promoType,
          promoImage: promoImage,
          authCusPromos: authCusPromos,
        );

  factory PromotionModel.fromJson(Map<String, dynamic> json) {
    return PromotionModel(
      promoID: json['promoID'] as int,
      promoName: json['promoName'] as String,
      promoDescription: json['promoDescription'] as String,
      promoDiscount: (json['promoDiscount'] as num).toDouble(),
      pointToGet: json['pointToGet'] as int,
      promoStart: DateTime.parse(json['promoStart'] as String),
      promoEnd: DateTime.parse(json['promoEnd'] as String),
      promoStatus: json['promoStatus'] as String,
      promoType: json['promoType'] as String,
      promoImage: json['promoImage'] as String,
      authCusPromos: json['authCusPromos'] as List<dynamic>?,
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'promoID': promoID,
      'promoName': promoName,
      'promoDescription': promoDescription,
      'promoDiscount': promoDiscount,
      'pointToGet': pointToGet,
      'promoStart': promoStart.toIso8601String(),
      'promoEnd': promoEnd.toIso8601String(),
      'promoStatus': promoStatus,
      'promoType': promoType,
      'promoImage': promoImage,
      'authCusPromos': authCusPromos,
    };
  }
}