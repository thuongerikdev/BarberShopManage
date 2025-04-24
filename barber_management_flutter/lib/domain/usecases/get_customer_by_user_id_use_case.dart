import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/domain/entities/customer.dart';
import 'package:barbermanagemobile/domain/repositories/customer_repository.dart';

class GetCustomerByUserIDUseCase {
  final CustomerRepository repository;

  GetCustomerByUserIDUseCase(this.repository);

  Future<Either<String, Customer>> call(int userID) async {
    try {
      final customer = await repository.getCustomerByUserID(userID);
      return Right(customer);
    } catch (e) {
      return Left(e.toString());
    }
  }
}