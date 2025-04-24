import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/repositories/booking_order_repository.dart';
import 'package:dartz/dartz.dart';

class CreateBookingOrderUseCase {
  final BookingOrderRepository repository;

  CreateBookingOrderUseCase(this.repository);

  Future<Either<String, void>> call(BookingCreateOrder order) async {
    try {
      await repository.createBookingOrder(order);
      return const Right(null);
    } catch (e) {
      return Left('Failed to create booking order: $e');
    }
  }
}