import 'package:barbermanagemobile/domain/repositories/booking_category_repository.dart';

class GetBookingCategoriesUseCase {
  final BookingCategoryRepository repository;

  GetBookingCategoriesUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call() async {
    return await repository.getBookingCategories();
  }
}