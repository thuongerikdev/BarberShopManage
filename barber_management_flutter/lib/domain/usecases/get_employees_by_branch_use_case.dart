import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetEmployeesByBranchUseCase {
  final BookingServiceRepository repository;

  GetEmployeesByBranchUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call(int branchID) async {
    return await repository.getEmployeesByBranch(branchID);
  }
}