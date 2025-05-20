import 'package:barbermanagemobile/domain/entities/customer_promotion.dart';

class CustomerPromotionModel extends CustomerPromotion {
  CustomerPromotionModel({
    required int customerID,
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
    required String cusPromoStatus,
  }) : super(
          customerID: customerID,
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
          cusPromoStatus: cusPromoStatus,
        );

  factory CustomerPromotionModel.fromJson(Map<String, dynamic> json) {
    return CustomerPromotionModel(
      customerID: json['customerID'] as int? ?? 0, // Default to 0 if null
      promoID: json['promoID'] as int? ?? 0, // Default to 0 if null
      promoName: json['promoName'] as String? ?? 'Unknown Promotion',
      promoDescription: json['promoDescription'] as String? ?? '',
      promoDiscount: (json['promoDiscount'] as num?)?.toDouble() ?? 0.0,
      pointToGet: json['pointToGet'] as int? ?? 0,
      promoStart: DateTime.tryParse(json['promoStart']?.toString() ?? '') ?? DateTime.now(),
      promoEnd: DateTime.tryParse(json['promoEnd']?.toString() ?? '') ?? DateTime.now(),
      promoStatus: json['promoStatus'] as String? ?? 'Unknown',
      promoType: json['promoType'] as String? ?? 'Unknown',
      promoImage: json['promoImage'] as String? ?? '',
      cusPromoStatus: json['cusPromoStatus'] as String? ?? 'Inactive',
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'customerID': customerID,
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
      'cusPromoStatus': cusPromoStatus,
    };
  }
}