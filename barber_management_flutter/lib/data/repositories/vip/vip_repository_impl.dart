import 'package:barbermanagemobile/data/datasources/vip/vip_remote_data_source.dart';
import 'package:barbermanagemobile/data/models/vip/vip_model.dart';
import 'package:barbermanagemobile/domain/entities/vip/vip.dart';
import 'package:barbermanagemobile/domain/repositories/vip_repository.dart';

class VipRepositoryImpl implements VipRepository {
  final VipRemoteDataSource remoteDataSource;

  VipRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Vip>> getVips() async {
    try {
      final List<VipModel> vipModels = await remoteDataSource.getVips();
      return vipModels.map((vipModel) => vipModel.toEntity()).toList();
    } catch (e) {
      throw Exception('Failed to fetch VIPs: $e');
    }
  }
}