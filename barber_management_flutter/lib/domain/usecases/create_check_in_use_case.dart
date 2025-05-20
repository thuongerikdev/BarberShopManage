import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/domain/repositories/check_in_repository.dart';

class CreateCheckInUseCase {
  final CheckInRepository repository;

  CreateCheckInUseCase(this.repository);

  Future<Either<String, Map<String, dynamic>>> call({
    required int customerID,
    required String checkInDate,
    required String checkInStatus,
    required String checkInType,
  }) async {
    try {
      final result = await repository.createCheckIn(
        customerID: customerID,
        checkInDate: checkInDate,
        checkInStatus: checkInStatus,
        checkInType: checkInType,
      );
      return Right(result);
    } catch (e) {
      return Left(e.toString());
    }
  }
}