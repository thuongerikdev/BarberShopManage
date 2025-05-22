import 'package:barbermanagemobile/domain/entities/vip/vip.dart';
import 'package:barbermanagemobile/domain/repositories/vip_repository.dart';

class GetVipsUseCase {
  final VipRepository repository;

  GetVipsUseCase(this.repository);

  Future<List<Vip>> call() async {
    return await repository.getVips();
  }
}