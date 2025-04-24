import 'package:barbermanagemobile/domain/entities/customer.dart';

abstract class CustomerRepository {
  Future<Customer> getCustomerByUserID(int userID);
}