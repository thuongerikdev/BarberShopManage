import 'package:barbermanagemobile/data/datasources/auth/employee_remote_data_source.dart';
import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';

class EmployeeRepositoryImpl implements EmployeeRepository {
  final EmployeeRemoteDataSource remoteDataSource;

  EmployeeRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Map<String, String>>> getEmployees() async {
    return await remoteDataSource.getEmployees();
  }

  @override
  Future<Map<String, dynamic>> getEmpByUserID(int userID) async {
    return await remoteDataSource.getEmpByUserID(userID);
  }

  @override
  Future<Map<String, dynamic>> getEmployeeById(int empID) async {
    return await remoteDataSource.getEmployeeById(empID);
  }
}