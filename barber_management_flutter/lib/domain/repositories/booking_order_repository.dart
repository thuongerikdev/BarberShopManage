import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/entities/booking_order.dart';

abstract class BookingOrderRepository {
  Future<List<BookingOrder>> getOrdersByCustomer(int custID);
  Future<void> createBookingOrder(BookingCreateOrder order);
  Future<void> deleteBookingOrder(int orderID);
  Future<String> payBookingOrder(int orderID);
}