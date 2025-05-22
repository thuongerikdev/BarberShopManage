import 'package:barbermanagemobile/domain/entities/social/branch.dart';
import 'package:barbermanagemobile/domain/repositories/branch_repository.dart';

class GetBranchesUseCase {
  final BranchRepository repository;

  GetBranchesUseCase(this.repository);

  Future<List<Branch>> call() async {
    return await repository.getBranches();
  }
}