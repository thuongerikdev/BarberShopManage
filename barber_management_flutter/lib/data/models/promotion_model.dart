import 'package:barbermanagemobile/domain/entities/promotion.dart';

class PromotionModel extends Promotion {
  PromotionModel({
    required int promoID,
    required String promoName,
    required String promoDescription,
    required double promoDiscount,
    required String promoImage,
  }) : super(
          promoID: promoID,
          promoName: promoName,
          promoDescription: promoDescription,
          promoDiscount: promoDiscount,
          promoImage: promoImage,
        );

  factory PromotionModel.fromJson(Map<String, dynamic> json) {
    return PromotionModel(
      promoID: json['promoID'] as int,
      promoName: json['promoName'] as String,
      promoDescription: json['promoDescription'] as String,
      promoDiscount: (json['promoDiscount'] as num).toDouble(),
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
      'promoImage': promoImage,
    };
  }
}