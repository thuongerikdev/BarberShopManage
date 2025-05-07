class Promotion {
  final int promoID;
  final String promoName;
  final String promoDescription;
  final double promoDiscount;
  final DateTime promoStart;
  final DateTime promoEnd;
  final String promoStatus;
  final String promoType;
  final String promoImage;

  Promotion({
    required this.promoID,
    required this.promoName,
    required this.promoDescription,
    required this.promoDiscount,
    required this.promoStart,
    required this.promoEnd,
    required this.promoStatus,
    required this.promoType,
    required this.promoImage,
  });

  factory Promotion.fromJson(Map<String, dynamic> json) {
    return Promotion(
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