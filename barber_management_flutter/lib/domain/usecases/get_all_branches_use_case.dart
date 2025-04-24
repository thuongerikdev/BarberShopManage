import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetAllBranchesUseCase {
  final BookingServiceRepository repository;

  GetAllBranchesUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call() async {
    return await repository.getAllBranches();
  }
}