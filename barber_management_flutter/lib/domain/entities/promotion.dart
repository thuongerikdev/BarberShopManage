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
  final List<dynamic>? authCusPromos;

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
    this.authCusPromos,
  });

  factory Promotion.fromJson(Map<String, dynamic> json) {
    return Promotion(
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