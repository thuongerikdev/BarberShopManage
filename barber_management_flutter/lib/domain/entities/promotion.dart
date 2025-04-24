class Promotion {
  final int promoID;
  final String promoName;
  final String promoDescription;
  final double promoDiscount;
  final String promoImage;

  Promotion({
    required this.promoID,
    required this.promoName,
    required this.promoDescription,
    required this.promoDiscount,
    required this.promoImage,
  });

  factory Promotion.fromJson(Map<String, dynamic> json) {
    return Promotion(
      promoID: json['promoID'] as int,
      promoName: json['promoName'] as String,
      promoDescription: json['promoDescription'] as String,
      promoDiscount: (json['promoDiscount'] as num).toDouble(),
      promoImage: json['promoImage'] as String,
    );
  }

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
