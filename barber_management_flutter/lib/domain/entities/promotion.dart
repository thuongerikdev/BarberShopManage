class Promotion {
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
  final bool isAssociatedWithCustomer; // New field
  final List<Map<String, dynamic>>? customerPromos; // New field, replaces authCusPromos

  Promotion({
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
    required this.isAssociatedWithCustomer,
    this.customerPromos,
  });

  factory Promotion.fromJson(Map<String, dynamic> json) {
    return Promotion(
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