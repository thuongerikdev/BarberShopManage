class BookingCategory {
  final int categoryId;
  final String categoryDescription;
  final String categoryImage;
  final double categoryPrice;

  BookingCategory({
    required this.categoryId,
    required this.categoryDescription,
    required this.categoryImage,
    required this.categoryPrice,
  });

  factory BookingCategory.fromJson(Map<String, dynamic> json) {
    return BookingCategory(
      categoryId: json['categoryId'] as int,
      categoryDescription: json['categoryDescription'] as String,
      categoryImage: json['categoryImage'] as String,
      categoryPrice: (json['categoryPrice'] as num).toDouble(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'categoryId': categoryId,
      'categoryDescription': categoryDescription,
      'categoryImage': categoryImage,
      'categoryPrice': categoryPrice,
    };
  }
}