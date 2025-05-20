import 'package:barbermanagemobile/data/datasources/check_in_remote_data_source.dart';
import 'package:barbermanagemobile/domain/repositories/check_in_repository.dart';

class CheckInRepositoryImpl implements CheckInRepository {
  final CheckInRemoteDataSource remoteDataSource;

  CheckInRepositoryImpl(this.remoteDataSource);

  @override
  Future<Map<String, dynamic>> createCheckIn({
    required int customerID,
    required String checkInDate,
    required String checkInStatus,
    required String checkInType,
  }) async {
    try {
      final result = await remoteDataSource.createCheckIn(
        customerID: customerID,
        checkInDate: checkInDate,
        checkInStatus: checkInStatus,
        checkInType: checkInType,
      );
      return result;
    } catch (e) {
      throw Exception('Failed to create check-in: $e');
    }
  }

  @override
  Future<List<Map<String, dynamic>>> getCheckInHistory(int customerID) async {
    try {
      final result = await remoteDataSource.getCheckInHistory(customerID);
      return result;
    } catch (e) {
      throw Exception('Failed to fetch check-in history: $e');
    }
  }
}