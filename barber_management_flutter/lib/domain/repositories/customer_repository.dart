import 'package:barbermanagemobile/domain/entities/auth/customer.dart';

abstract class CustomerRepository {
  Future<Customer> getCustomerByUserID(int userID);
}