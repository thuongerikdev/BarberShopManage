import 'package:barbermanagemobile/domain/entities/vip/promotion.dart';

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
    required bool isAssociatedWithCustomer,
    List<Map<String, dynamic>>? customerPromos,
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
          isAssociatedWithCustomer: isAssociatedWithCustomer,
          customerPromos: customerPromos,
        );

  factory PromotionModel.fromJson(Map<String, dynamic> json) {
    return PromotionModel(
      promoID: json['promoID'] as int? ?? 0,
      promoName: json['promoName'] as String? ?? 'Unknown Promotion',
      promoDescription: json['promoDescription'] as String? ?? '',
      promoDiscount: (json['promoDiscount'] as num?)?.toDouble() ?? 0.0,
      pointToGet: json['pointToGet'] as int? ?? 0,
      promoStart: DateTime.tryParse(json['promoStart']?.toString() ?? '') ?? DateTime.now(),
      promoEnd: DateTime.tryParse(json['promoEnd']?.toString() ?? '') ?? DateTime.now(),
      promoStatus: json['promoStatus'] as String? ?? 'Unknown',
      promoType: json['promoType'] as String? ?? 'Unknown',
      promoImage: json['promoImage'] as String? ?? '',
      isAssociatedWithCustomer: json['isAssociatedWithCustomer'] as bool? ?? false,
      customerPromos: (json['customerPromos'] as List<dynamic>?)?.cast<Map<String, dynamic>>(),
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
      'isAssociatedWithCustomer': isAssociatedWithCustomer,
      'customerPromos': customerPromos,
    };
  }
}