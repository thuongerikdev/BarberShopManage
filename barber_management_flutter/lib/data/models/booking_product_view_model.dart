import 'package:barbermanagemobile/domain/entities/booking_product.dart';

class BookingProductViewModel {
  final String productName;
  final double productPrice;
  final String productImage;

  BookingProductViewModel({
    required this.productName,
    required this.productPrice,
    required this.productImage,
  });

  factory BookingProductViewModel.fromBookingProduct(BookingProduct product) {
    return BookingProductViewModel(
      productName: product.productName,
      productPrice: product.productPrice,
      productImage: product.productImage.isNotEmpty ? product.productImage : 'https://via.placeholder.com/120',
    );
  }
}