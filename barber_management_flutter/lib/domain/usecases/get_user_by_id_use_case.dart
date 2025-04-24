import 'package:dartz/dartz.dart';
import '../../data/models/user_model.dart';
import '../../domain/repositories/auth_repository.dart';

class GetUserByIDUseCase {
  final AuthRepository repository;

  GetUserByIDUseCase(this.repository);

  Future<Either<String, UserModel>> call(int userID) async {
    return await repository.getUserByID(userID);
  }
}