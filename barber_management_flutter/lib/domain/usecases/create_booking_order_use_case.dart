import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class CreateBookingOrderUseCase {
  final BookingServiceRepository repository;

  CreateBookingOrderUseCase(this.repository);

  Future<void> call(BookingCreateOrder order) async {
    await repository.createBookingOrder(order);
  }
}
