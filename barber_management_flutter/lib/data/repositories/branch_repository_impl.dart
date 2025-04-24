import 'package:barbermanagemobile/data/datasources/branch_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/branch.dart';
import 'package:barbermanagemobile/domain/repositories/branch_repository.dart';

class BranchRepositoryImpl implements BranchRepository {
  final BranchRemoteDataSource remoteDataSource;

  BranchRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Branch>> getBranches() async {
    try {
      final branches = await remoteDataSource.getBranches();
      return branches;
    } catch (e) {
      throw Exception('Failed to fetch branches: $e');
    }
  }
}