abstract class EmployeeRepository {
  Future<List<Map<String, String>>> getEmployees();
  Future<Map<String, dynamic>> getEmpByUserID(int userID);
  Future<Map<String, dynamic>> getEmployeeById(int empID);
}