import 'package:dartz/dartz.dart';
import '../entities/user.dart';
import '../repositories/auth_repository.dart';

class LoginUseCase {
  final AuthRepository repository;

  LoginUseCase(this.repository);

  Future<Either<String, User>> call(String email, String password) async {
    final result = await repository.login(email, password);
    return result.fold(
      (failure) => Left(failure),
      (userModel) => Right(User(
        name: userModel.name,
        userId: userModel.userId,
        token: userModel.token,
      )),
    );
  }
}