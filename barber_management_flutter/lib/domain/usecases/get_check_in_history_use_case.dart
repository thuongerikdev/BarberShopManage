import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/domain/repositories/check_in_repository.dart';

class GetCheckInHistoryUseCase {
  final CheckInRepository repository;

  GetCheckInHistoryUseCase(this.repository);

  Future<Either<String, List<Map<String, dynamic>>>> call(int customerID) async {
    try {
      final result = await repository.getCheckInHistory(customerID);
      return Right(result);
    } catch (e) {
      return Left(e.toString());
    }
  }
}