import 'package:barbermanagemobile/data/datasources/vip_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/vip.dart';
import 'package:barbermanagemobile/domain/repositories/vip_repository.dart';

class VipRepositoryImpl implements VipRepository {
  final VipRemoteDataSource remoteDataSource;

  VipRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Vip>> getVips() async {
    try {
      final vips = await remoteDataSource.getVips();
      return vips;
    } catch (e) {
      throw Exception('Failed to fetch VIPs: $e');
    }
  }
}