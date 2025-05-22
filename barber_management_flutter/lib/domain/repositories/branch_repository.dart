import 'package:barbermanagemobile/domain/entities/social/branch.dart';

abstract class BranchRepository {
  Future<List<Branch>> getBranches();
}