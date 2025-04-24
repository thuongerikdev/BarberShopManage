import 'package:barbermanagemobile/data/datasources/booking_order_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/entities/booking_order.dart';
import 'package:barbermanagemobile/domain/repositories/booking_order_repository.dart';

class BookingOrderRepositoryImpl implements BookingOrderRepository {
  final BookingOrderRemoteDataSource remoteDataSource;

  BookingOrderRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<BookingOrder>> getOrdersByCustomer(int custID) async {
    try {
      final orders = await remoteDataSource.getOrdersByCustomer(custID);
      return orders;
    } catch (e) {
      throw Exception('Failed to fetch orders: $e');
    }
  }

  @override
  Future<void> createBookingOrder(BookingCreateOrder order) async {
    try {
      await remoteDataSource.createBookingOrder(order);
    } catch (e) {
      throw Exception('Failed to create booking order: $e');
    }
  }

  @override
  Future<void> deleteBookingOrder(int orderID) async {
    try {
      await remoteDataSource.deleteBookingOrder(orderID);
    } catch (e) {
      throw Exception('Failed to delete booking order: $e');
    }
  }

  @override
  Future<String> payBookingOrder(int orderID) async {
    try {
      return await remoteDataSource.payBookingOrder(orderID);
    } catch (e) {
      throw Exception('Failed to initiate payment: $e');
    }
  }
}

