import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';

abstract class BookingServiceRepository {
  Future<List<Map<String, dynamic>>> getAllBranches();
  Future<List<Map<String, dynamic>>> getEmployeesByBranch(int branchID);
  Future<List<Map<String, dynamic>>> getEmployeesByDate(DateTime date);
  Future<List<Map<String, dynamic>>> getAllServices();
  Future<void> createBookingOrder(BookingCreateOrder order);
}