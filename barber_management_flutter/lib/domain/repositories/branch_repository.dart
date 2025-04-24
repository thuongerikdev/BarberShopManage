import 'package:barbermanagemobile/domain/entities/branch.dart';

abstract class BranchRepository {
  Future<List<Branch>> getBranches();
}