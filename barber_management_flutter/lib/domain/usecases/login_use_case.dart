import 'package:dartz/dartz.dart';
import 'package:barbermanagemobile/data/models/auth/user_model.dart';
import 'package:barbermanagemobile/domain/repositories/auth_repository.dart';

class LoginUseCase {
  final AuthRepository repository;

  LoginUseCase(this.repository);

  Future<Either<String, UserModel>> call(String email, String password) async {
    return await repository.login(email, password);
  }
}