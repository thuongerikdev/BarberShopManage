// import 'package:barbermanagemobile/domain/entities/customer_promotion.dart';

class CustomerPromotion {
  final int customerID;
  final int promoID;
  final String promoName;
  final String promoDescription;
  final double promoDiscount;
  final int pointToGet;
  final DateTime promoStart;
  final DateTime promoEnd;
  final String promoStatus;
  final String promoType;
  final String promoImage;
  final String cusPromoStatus;

  CustomerPromotion({
    required this.customerID,
    required this.promoID,
    required this.promoName,
    required this.promoDescription,
    required this.promoDiscount,
    required this.pointToGet,
    required this.promoStart,
    required this.promoEnd,
    required this.promoStatus,
    required this.promoType,
    required this.promoImage,
    required this.cusPromoStatus,
  });

  factory CustomerPromotion.fromJson(Map<String, dynamic> json) {
    return CustomerPromotion(
      customerID: json['customerID'] as int? ?? 0, // Handle null with default
      promoID: json['promoID'] as int? ?? 0, // Handle null with default
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