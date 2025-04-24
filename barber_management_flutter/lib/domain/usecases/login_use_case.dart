import 'package:dartz/dartz.dart';
// import 'package:barbermanagemobile/data/models/user_model.dart';
import 'package:barbermanagemobile/domain/entities/user.dart';
import 'package:barbermanagemobile/domain/repositories/auth_repository.dart';

class LoginUseCase {
  final AuthRepository repository;

  LoginUseCase(this.repository);

  Future<Either<String, User>> call(String email, String password) async {
    final result = await repository.login(email, password);
    return result.fold(
      (failure) => Left(failure),
      (userModel) => Right(userModel),
    );
  }
}