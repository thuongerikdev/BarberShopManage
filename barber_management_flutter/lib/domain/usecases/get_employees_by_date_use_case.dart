import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetEmployeesByDateUseCase {
  final BookingServiceRepository repository;

  GetEmployeesByDateUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call(DateTime date, int branchID, String typeOfEmp) async {
    return await repository.getEmployeesByDate(date, branchID, typeOfEmp);
  }
}