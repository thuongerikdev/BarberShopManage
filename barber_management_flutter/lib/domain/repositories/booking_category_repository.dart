abstract class BookingCategoryRepository {
  Future<List<Map<String, dynamic>>> getBookingCategories();
}