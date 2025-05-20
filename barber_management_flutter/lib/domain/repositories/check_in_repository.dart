abstract class CheckInRepository {
  Future<Map<String, dynamic>> createCheckIn({
    required int customerID,
    required String checkInDate,
    required String checkInStatus,
    required String checkInType,
  });

  Future<List<Map<String, dynamic>>> getCheckInHistory(int customerID); // New method
}