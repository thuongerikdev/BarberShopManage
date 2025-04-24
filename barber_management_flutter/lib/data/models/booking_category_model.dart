import 'package:barbermanagemobile/domain/entities/booking_category.dart';

class BookingCategoryModel extends BookingCategory {
  BookingCategoryModel({
    required int categoryId,
    required String categoryDescription,
    required String categoryImage,
    required double categoryPrice,
  }) : super(
          categoryId: categoryId,
          categoryDescription: categoryDescription,
          categoryImage: categoryImage,
          categoryPrice: categoryPrice,
        );

  factory BookingCategoryModel.fromJson(Map<String, dynamic> json) {
    return BookingCategoryModel(
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