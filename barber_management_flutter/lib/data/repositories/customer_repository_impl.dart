import 'package:barbermanagemobile/data/datasources/customer_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/customer.dart';
import 'package:barbermanagemobile/domain/repositories/customer_repository.dart';

class CustomerRepositoryImpl implements CustomerRepository {
  final CustomerRemoteDataSource remoteDataSource;

  CustomerRepositoryImpl(this.remoteDataSource);

  @override
  Future<Customer> getCustomerByUserID(int userID) async {
    try {
      final customer = await remoteDataSource.getCustomerByUserID(userID);
      return customer;
    } catch (e) {
      throw Exception('Failed to fetch customer: $e');
    }
  }
}