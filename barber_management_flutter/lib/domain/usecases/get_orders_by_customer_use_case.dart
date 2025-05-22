import 'package:barbermanagemobile/domain/entities/booking/booking_order.dart';
import 'package:barbermanagemobile/domain/repositories/booking_order_repository.dart';

class GetOrdersByCustomerUseCase {
  final BookingOrderRepository repository;

  GetOrdersByCustomerUseCase(this.repository);

  Future<List<BookingOrder>> call(int custID) async {
    return await repository.getOrdersByCustomer(custID);
  }
}