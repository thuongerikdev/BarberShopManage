// lib/data/repositories/employee_repository_impl.dart
import 'package:barbermanagemobile/data/datasources/employee_remote_data_source.dart';
import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';

class EmployeeRepositoryImpl implements EmployeeRepository {
  final EmployeeRemoteDataSource remoteDataSource;

  EmployeeRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Map<String, String>>> getEmployees() async {
    return await remoteDataSource.getEmployees();
  }
}